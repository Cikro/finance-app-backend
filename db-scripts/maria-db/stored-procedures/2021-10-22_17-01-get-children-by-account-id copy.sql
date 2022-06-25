DELIMITER |
DROP PROCEDURE IF EXISTS GetChildrenByAccountId|
CREATE PROCEDURE
    GetChildrenByAccountId(accountId int UNSIGNED)
    READS SQL DATA
    BEGIN
        SELECT * FROM accounts WHERE parent_account = accountId;
    END|
DELIMITER ;