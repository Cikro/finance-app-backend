DELIMITER |
DROP PROCEDURE IF EXISTS UpdateAccount|
CREATE PROCEDURE
    UpdateAccount(IN accountId int UNSIGNED, IN accountName varchar(255), IN accountDescription varchar(255), IN accountClosed BOOL) 
    READS SQL DATA
    BEGIN
        UPDATE accounts SET name = accountName, description = accountDescription, closed=accountClosed WHERE id = accountId;
        SELECT * FROM accounts WHERE id = accountId;
    END|
DELIMITER ;