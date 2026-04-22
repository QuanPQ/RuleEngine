# RuleEngine - .NET Core 8.0

Rule Engine cho phép định nghĩa và thực thi các business rules thông qua cấu hình trong database, không cần thay đổi code.

## Mục lục

- Kiến trúc
- API Endpoints
- Database Schema
- Condition JSON
- Action JSON
- Cách tạo Rule mới
- Ví dụ thực tế
- Cache
- Cấu hình

---

## Kiến trúc

Request JSON
    |
    v
RulesController --> RuleService --> RuleEngineProcessor
                                        |
                    +-------------------+-------------------+
                    v                   v                   v
            LoadCondition()      LoadActions()        SaveLog()
                    |                   |
                    v                   v
        RuleConditionEvaluator   RuleActionExecutor

Luồng xử lý:
1. Nhận request JSON với targetType + targetCode
2. Load danh sách rules từ RuleAssignmentView (có cache)
3. Với mỗi rule theo thứ tự Priority:
   - Evaluate condition -> nếu true thì execute actions
   - Nếu StopOnFirstFail = true và có violation -> dừng
4. Batch save execution logs
5. Trả về response

---

## API Endpoints

### Evaluate Rules
POST /api/v1/rules/evaluate

Request body: JSON object bất kỳ, bắt buộc có targetType và targetCode ở root level.

### Cache Management
DELETE /api/v1/rules/cache                                    -- Xóa toàn bộ cache
DELETE /api/v1/rules/cache/target/{targetType}/{targetCode}   -- Xóa theo target
DELETE /api/v1/rules/cache/rule/{ruleId}                      -- Xóa theo ruleId

---

## Condition Types: AND, OR, NOT, SIMPLE

## Operators:
EQ, NEQ, GT, GTE, LT, LTE, IN, NOT_IN, BETWEEN,
IS_NULL, IS_NOT_NULL, CONTAINS, NOT_CONTAINS,
STARTS_WITH, ENDS_WITH, MATCHES, NOT_MATCHES,
IS_VALID_DATE, IS_NOT_VALID_DATE, DB_FUNCTION

## Action Types: REQUIRE_FIELD, REJECT, SET_VALUE, TRIGGER_RULE

## SET_VALUE uses DynamicExpresso (C# expression evaluation)
Built-in variables: today (DateTime.Today), now (DateTime.Now)
Referenced types: Math, DateTime, TimeSpan, Convert, string, decimal, int, double

## Cache: IMemoryCache with 10-minute TTL
Keys: rules:{targetType}:{targetCode}, cond:{ruleId}, actions:{ruleId}

## Dependencies:
- .NET 10.0
- Microsoft.EntityFrameworkCore.SqlServer
- DynamicExpresso.Core
- Swashbuckle.AspNetCore
