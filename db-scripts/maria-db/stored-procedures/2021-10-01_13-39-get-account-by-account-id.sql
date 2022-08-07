DELIMITER |
DROP PROCEDURE IF EXISTS GetAccountByAccountId|
CREATE PROCEDURE
    GetAccountByAccountId(accountId int UNSIGNED)
    READS SQL DATA
    BEGIN
        SELECT * FROM accounts WHERE id = accountId;
    END|
DELIMITER ;