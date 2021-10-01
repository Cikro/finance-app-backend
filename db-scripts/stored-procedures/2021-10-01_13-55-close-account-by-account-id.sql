DELIMITER |
DROP PROCEDURE IF EXISTS CloseAccountByAccountId|
CREATE PROCEDURE
    CloseAccountByAccountId int UNSIGNED)
    READS SQL DATA
    BEGIN
        UPDATE accounts SET closed = 1 WHERE id = accountId;
    END|
DELIMITER ;