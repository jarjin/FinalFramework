using System.Collections.Generic;

namespace FTSwfTools.SwfTags {
	public class DefineSceneAndFrameLabelDataTag : SwfTagBase {
		public struct SceneOffsetData {
			public uint   Offset;
			public string Name;
		}

		public struct FrameLabelData {
			public uint   Number;
			public string Label;
		}

		public List<SceneOffsetData> Scenes;
		public List<FrameLabelData>  Frames;

		public override SwfTagType TagType {
			get { return SwfTagType.DefineSceneAndFrameLabelData; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"DefineSceneAndFrameLabelDataTag. " +
				"Scenes: {0}, Frames: {1}",
				Scenes.Count, Frames.Count);
		}

		public static DefineSceneAndFrameLabelDataTag Create(SwfStreamReader reader) {
			var scene_count = reader.ReadEncodedU32();
			var scenes      = new List<SceneOffsetData>((int)scene_count);
			for ( var i = 0; i < scene_count; ++i ) {
				scenes.Add(new SceneOffsetData{
					Offset = reader.ReadEncodedU32(),
					Name   = reader.ReadString()});
			}
			var frame_count = reader.ReadEncodedU32();
			var frames      = new List<FrameLabelData>((int)frame_count);
			for ( var i = 0; i < frame_count; ++i ) {
				frames.Add(new FrameLabelData{
					Number = reader.ReadEncodedU32(),
					Label  = reader.ReadString()});
			}
			return new DefineSceneAndFrameLabelDataTag{
				Scenes = scenes,
				Frames = frames};
		}
	}
}