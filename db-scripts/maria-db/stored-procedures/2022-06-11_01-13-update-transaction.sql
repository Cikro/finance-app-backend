DELIMITER |
DROP PROCEDURE IF EXISTS UpdateTransaction|
CREATE PROCEDURE
    UpdateTransaction(IN transactionId int UNSIGNED, IN notes varchar(100)) 
    READS SQL DATA
    BEGIN
        UPDATE transactions SET notes = notes WHERE id = transactionId;
        SELECT * FROM transactions WHERE id = transactionId;
    END|
DELIMITER ;