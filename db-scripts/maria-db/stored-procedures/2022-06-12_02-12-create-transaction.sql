DELIMITER |
DROP PROCEDURE IF EXISTS CreateTransaction|
CREATE PROCEDURE
    CreateTransaction(IN accountId int UNSIGNED, type tinyint unsigned, IN notes VARCHAR(100), In transaction_date date, IN amount DECIMAL(15,2), IN serverGenerated boolean )
    READS SQL DATA
    BEGIN
        INSERT INTO transactions
             (accountId, amount, type, notes, transaction_date, server_generated) 
             VALUES (accountId, amount, type, notes, transaction_date, server_generated);
        SELECT * FROM journal_entries WHERE id=(SELECT LAST_INSERT_ID());
    END|
DELIMITER ;