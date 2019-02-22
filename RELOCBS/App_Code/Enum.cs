public enum MessageType
{
    MESSAGE,
    ERROR
}

public enum MessageDisplayType
{
    POPUP,
    INLINE
}

public enum SafeTextType
{
    TRIMMEDTEXT = 1,
    WITHOUTCTRLCHAR = 2,
    TEXTTOXML = 3,
    XMLTOTEXT = 4
}

public enum EValueType
{
    STRING = 1,
    INT = 2,
    DOUBLE = 3,
    DATE = 4
}

public enum PermissionType
{
    VIEW = 1,
    ADD = 2,
    EDIT = 3,
    DELETE = 4,
    NONE = 99
}

public enum QueryType
{
    QueryText,
    Procedure
}

public enum QueryReturnType
{
    DataTable,
    DataSet,
    SingleValue
}

public enum ProcedureReturnType
{
    DataTable,
    DataSet,
    SingleValue
}

public enum ParameterOF
{
    QUERY,
    PROCEDURE
}

public enum OpenMode
{
    WRITE = 0,
    READ = 1
}

public enum DBInstance
{
    RELOCBS
}

public enum LogLevel
{
    Debug = 10,
    Information = 20,
    Warning = 30,
    Error = 40,
    Fatal = 50
}

//public enum NotifyType
//{
//    Info = 0,
//    Success = 1,
//    Warning = 2,
//    Error = 3
//}