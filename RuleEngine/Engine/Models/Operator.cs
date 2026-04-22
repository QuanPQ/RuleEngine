namespace RuleEngine.Engine.Models;

public enum Operator
{
    EQ, NEQ, GT, GTE, LT, LTE,
    IN, NOT_IN, BETWEEN,
    IS_NULL, IS_NOT_NULL,
    CONTAINS, NOT_CONTAINS,
    STARTS_WITH, ENDS_WITH,
    MATCHES, NOT_MATCHES,
    IS_VALID_DATE, IS_NOT_VALID_DATE,
    DB_FUNCTION
}
