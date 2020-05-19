using System.Collections.Generic;
using FTSwfTools.SwfTags;

namespace FTSwfTools.SwfTypes {
	public struct SwfControlTags {
		public List<SwfTagBase> Tags;

		public static SwfControlTags identity {
			get {
				return new SwfControlTags {
					Tags = new List<SwfTagBase>()};
			}
		}

		public static SwfControlTags Read(SwfStreamReader reader) {
			var control_tags = SwfControlTags.identity;
			while ( true ) {
				var tag = SwfTagBase.Read(reader);
				if ( tag.TagType == SwfTagType.End ) {
					break;
				}
				control_tags.Tags.Add(tag);
			}
			return control_tags;
		}

		public override string ToString() {
			return string.Format(
				"SwfControlTags. " +
				"Tags: {0}",
				Tags.Count);
		}
	}
}