using System.Collections.Generic;

namespace FTSwfTools.SwfTags {
	public class ExportAssetsTag : SwfTagBase {
		public struct AssetTagData {
			public ushort Tag;
			public string Name;
		}

		public List<AssetTagData> AssetTags;

		public override SwfTagType TagType {
			get { return SwfTagType.ExportAssets; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"ExportAssetsTag. " +
				"AssetTags: {0}",
				AssetTags.Count);
		}

		public static ExportAssetsTag Create(SwfStreamReader reader) {
			var asset_tag_count = reader.ReadUInt16();
			var asset_tags      = new List<AssetTagData>(asset_tag_count);
			for ( var i = 0; i < asset_tag_count; ++i ) {
				asset_tags.Add(new AssetTagData{
					Tag  = reader.ReadUInt16(),
					Name = reader.ReadString()});
			}
			return new ExportAssetsTag{
				AssetTags = asset_tags};
		}
	}
}