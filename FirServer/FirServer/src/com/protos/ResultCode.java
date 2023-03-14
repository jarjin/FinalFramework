// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: common.proto

package com.protos;

/**
 * Protobuf enum {@code pb_common.ResultCode}
 */
public enum ResultCode
    implements com.google.protobuf.ProtocolMessageEnum {
  /**
   * <pre>
   *操作成功
   * </pre>
   *
   * <code>Success = 0;</code>
   */
  Success(0),
  /**
   * <pre>
   *操作失败
   * </pre>
   *
   * <code>Failed = 1;</code>
   */
  Failed(1),
  UNRECOGNIZED(-1),
  ;

  /**
   * <pre>
   *操作成功
   * </pre>
   *
   * <code>Success = 0;</code>
   */
  public static final int Success_VALUE = 0;
  /**
   * <pre>
   *操作失败
   * </pre>
   *
   * <code>Failed = 1;</code>
   */
  public static final int Failed_VALUE = 1;


  public final int getNumber() {
    if (this == UNRECOGNIZED) {
      throw new java.lang.IllegalArgumentException(
          "Can't get the number of an unknown enum value.");
    }
    return value;
  }

  /**
   * @deprecated Use {@link #forNumber(int)} instead.
   */
  @java.lang.Deprecated
  public static ResultCode valueOf(int value) {
    return forNumber(value);
  }

  public static ResultCode forNumber(int value) {
    switch (value) {
      case 0: return Success;
      case 1: return Failed;
      default: return null;
    }
  }

  public static com.google.protobuf.Internal.EnumLiteMap<ResultCode>
      internalGetValueMap() {
    return internalValueMap;
  }
  private static final com.google.protobuf.Internal.EnumLiteMap<
      ResultCode> internalValueMap =
        new com.google.protobuf.Internal.EnumLiteMap<ResultCode>() {
          public ResultCode findValueByNumber(int number) {
            return ResultCode.forNumber(number);
          }
        };

  public final com.google.protobuf.Descriptors.EnumValueDescriptor
      getValueDescriptor() {
    return getDescriptor().getValues().get(ordinal());
  }
  public final com.google.protobuf.Descriptors.EnumDescriptor
      getDescriptorForType() {
    return getDescriptor();
  }
  public static final com.google.protobuf.Descriptors.EnumDescriptor
      getDescriptor() {
    return com.protos.pb_common.getDescriptor()
        .getEnumTypes().get(0);
  }

  private static final ResultCode[] VALUES = values();

  public static ResultCode valueOf(
      com.google.protobuf.Descriptors.EnumValueDescriptor desc) {
    if (desc.getType() != getDescriptor()) {
      throw new java.lang.IllegalArgumentException(
        "EnumValueDescriptor is not for this type.");
    }
    if (desc.getIndex() == -1) {
      return UNRECOGNIZED;
    }
    return VALUES[desc.getIndex()];
  }

  private final int value;

  private ResultCode(int value) {
    this.value = value;
  }

  // @@protoc_insertion_point(enum_scope:pb_common.ResultCode)
}

