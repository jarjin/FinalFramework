namespace FTSwfTools.SwfTags {
	public interface SwfTagVisitor<TArg, TResult> {
		TResult Visit(PlaceObjectTag                  tag, TArg arg);
		TResult Visit(PlaceObject2Tag                 tag, TArg arg);
		TResult Visit(PlaceObject3Tag                 tag, TArg arg);
		TResult Visit(RemoveObjectTag                 tag, TArg arg);
		TResult Visit(RemoveObject2Tag                tag, TArg arg);
		TResult Visit(ShowFrameTag                    tag, TArg arg);

		TResult Visit(SetBackgroundColorTag           tag, TArg arg);
		TResult Visit(FrameLabelTag                   tag, TArg arg);
		TResult Visit(ProtectTag                      tag, TArg arg);
		TResult Visit(EndTag                          tag, TArg arg);
		TResult Visit(ExportAssetsTag                 tag, TArg arg);
		TResult Visit(EnableDebuggerTag               tag, TArg arg);
		TResult Visit(EnableDebugger2Tag              tag, TArg arg);
		TResult Visit(ScriptLimitsTag                 tag, TArg arg);
		TResult Visit(SymbolClassTag                  tag, TArg arg);
		TResult Visit(MetadataTag                     tag, TArg arg);
		TResult Visit(DefineSceneAndFrameLabelDataTag tag, TArg arg);

		TResult Visit(DoABCTag                        tag, TArg arg);

		TResult Visit(DefineShapeTag                  tag, TArg arg);
		TResult Visit(DefineShape2Tag                 tag, TArg arg);
		TResult Visit(DefineShape3Tag                 tag, TArg arg);
		TResult Visit(DefineShape4Tag                 tag, TArg arg);

		TResult Visit(DefineBitsLosslessTag           tag, TArg arg);
		TResult Visit(DefineBitsLossless2Tag          tag, TArg arg);

		TResult Visit(DefineSpriteTag                 tag, TArg arg);

		TResult Visit(FileAttributesTag               tag, TArg arg);
		TResult Visit(EnableTelemetryTag              tag, TArg arg);
		TResult Visit(DefineBinaryDataTag             tag, TArg arg);

		TResult Visit(UnknownTag                      tag, TArg arg);
		TResult Visit(UnsupportedTag                  tag, TArg arg);
	}
}