using Bulb.DataType;

namespace Bulb;

public record Variable(string Name, int StackLocation, BaseDataType DataType);