using FTSwfTools.SwfTypes;

namespace FTSwfTools.SwfTags {
	public class DefineBitsLossless2Tag : SwfTagBase {
		public ushort CharacterId;
		public byte   BitmapFormat;
		public ushort BitmapWidth;
		public ushort BitmapHeight;
		public ushort BitmapColorTableSize;
		public byte[] ZlibBitmapData;

		public override SwfTagType TagType {
			get { return SwfTagType.DefineBitsLossless2; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"DefineBitsLossless2Tag. " +
				"CharacterId: {0}, BitmapFormat: {1}, Width: {2}, Height: {3}",
				CharacterId, BitmapFormat, BitmapWidth, BitmapHeight);
		}

		public static DefineBitsLossless2Tag Create(SwfStreamReader reader) {
			var tag          = new DefineBitsLossless2Tag();
			tag.CharacterId  = reader.ReadUInt16();
			tag.BitmapFormat = reader.ReadByte();
			tag.BitmapWidth  = reader.ReadUInt16();
			tag.BitmapHeight = reader.ReadUInt16();
			if ( tag.BitmapFormat == 3 ) {
				tag.BitmapColorTableSize = (ushort)(reader.ReadByte() + 1);
			}
			tag.ZlibBitmapData = reader.ReadRest();
			return tag;
		}

		public byte[] ToARGB32() {
			var result     = new byte[BitmapWidth * BitmapHeight * 4];
			var swf_reader = SwfStreamReader.DecompressZBytesToReader(ZlibBitmapData);
			if ( BitmapFormat == 3 ) {
				var palette = new SwfColor[BitmapColorTableSize];
				for ( var i = 0; i < palette.Length; ++i ) {
					palette[i] = SwfColor.Read(swf_reader, true);
				}
				var palette_pitch = BitmapWidth % 4 == 0
					? BitmapWidth
					: BitmapWidth + (4 - BitmapWidth % 4);
				var palette_indices = swf_reader.ReadRest();
				for ( var i = 0; i < BitmapHeight; ++i ) {
					for ( var j = 0; j < BitmapWidth; ++j ) {
						var result_index  = j + i * BitmapWidth;
						var palette_index = palette_indices[j + i * palette_pitch];
						var palette_color = palette[palette_index];
						result[result_index * 4 + 0] = palette_color.A;
						result[result_index * 4 + 1] = palette_color.R;
						result[result_index * 4 + 2] = palette_color.G;
						result[result_index * 4 + 3] = palette_color.B;
					}
				}
			} else if ( BitmapFormat == 5 ) {
				for ( var i = 0; i < BitmapWidth * BitmapHeight; ++i ) {
					var pix32 = swf_reader.ReadUInt32();
					result[i * 4 + 0] = (byte)((pix32      ) & 0xFF);
					result[i * 4 + 1] = (byte)((pix32 >>  8) & 0xFF);
					result[i * 4 + 2] = (byte)((pix32 >> 16) & 0xFF);
					result[i * 4 + 3] = (byte)((pix32 >> 24) & 0xFF);
				}
			} else {
				throw new System.Exception(string.Format(
					"Incorrect DefineBitsLossless2 format: {0}",
					BitmapFormat));
			}
			return result;
		}
	}
}