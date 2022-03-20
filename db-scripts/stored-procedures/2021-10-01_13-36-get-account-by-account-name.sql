DELIMITER |
DROP PROCEDURE IF EXISTS GetAccountByAccountName|
CREATE PROCEDURE
    GetAccountByAccountName(userId int UNSIGNED, accountName VarChar(50))
    READS SQL DATA
    BEGIN
        SELECT * FROM accounts WHERE user_id = userId and name = accountName;
    END|
DELIMITER ;