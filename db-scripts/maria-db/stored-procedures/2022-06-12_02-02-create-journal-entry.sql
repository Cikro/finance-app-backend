DELIMITER |
DROP PROCEDURE IF EXISTS CreateJournalEntry|
CREATE PROCEDURE
    CreateJournalEntry(IN userId int UNSIGNED, IN amount DECIMAL(15,2), IN corrected boolean, IN serverGenerated boolean )
    READS SQL DATA
    BEGIN
        INSERT INTO journal_entries (user_id, amount, corrected, server_generated) VALUES (userId, amount, corrected, server_generated);
        SELECT * FROM journal_entries WHERE id=(SELECT LAST_INSERT_ID());
    END|
DELIMITER ;