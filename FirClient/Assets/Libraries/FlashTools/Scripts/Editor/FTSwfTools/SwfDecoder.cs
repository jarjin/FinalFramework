using System.IO;
using System.Collections.Generic;

using FTSwfTools.SwfTags;
using FTSwfTools.SwfTypes;

namespace FTSwfTools {
	public class SwfDecoder {
		public SwfShortHeader   OriginalHeader;
		public SwfLongHeader    UncompressedHeader;
		public List<SwfTagBase> Tags = new List<SwfTagBase>();

		public SwfDecoder(string swf_path) : this(swf_path, null) {
		}

		public SwfDecoder(string swf_path, System.Action<float> progress_act) {
			var raw_data            = File.ReadAllBytes(swf_path);
			var uncompressed_stream = DecompressSwfData(raw_data);
			DecodeSwf(new SwfStreamReader(uncompressed_stream), progress_act);
		}

		MemoryStream DecompressSwfData(byte[] raw_swf_data) {
			var raw_reader = new SwfStreamReader(raw_swf_data);
			OriginalHeader = SwfShortHeader.Read(raw_reader);
			switch ( OriginalHeader.Format ) {
			case "FWS":
				return new MemoryStream(raw_swf_data);
			case "CWS":
				var rest_stream = SwfStreamReader.DecompressZBytes(
					raw_reader.ReadRest());
				var new_short_header = new SwfShortHeader{
					Format     = "FWS",
					Version    = OriginalHeader.Version,
					FileLength = OriginalHeader.FileLength};
				var uncompressed_stream = new MemoryStream();
				SwfShortHeader.Write(new_short_header, uncompressed_stream);
				rest_stream.WriteTo(uncompressed_stream);
				uncompressed_stream.Position = 0;
				return uncompressed_stream;
			default:
				throw new System.Exception(string.Format(
					"Unsupported swf format: {0}", OriginalHeader.Format));
			}
		}

		void DecodeSwf(SwfStreamReader reader, System.Action<float> progress_act) {
			UncompressedHeader = SwfLongHeader.Read(reader);
			while ( !reader.IsEOF ) {
				if ( progress_act != null ) {
					progress_act((float)(reader.Position + 1) / reader.Length);
				}
				var tag = SwfTagBase.Read(reader);
				if ( tag.TagType == SwfTagType.End ) {
					break;
				}
				Tags.Add(tag);
			}
		}
	}
}