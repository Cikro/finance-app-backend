DELIMITER |
DROP PROCEDURE IF EXISTS CloseAccountByAccountId|
CREATE PROCEDURE
    CloseAccountByAccountId(accountId int UNSIGNED)
    READS SQL DATA
    BEGIN
        CREATE TEMPORARY TABLE tempAccountIds SELECT id FROM accounts WHERE id=accountId OR parent_account=accountId AND closed = 0;
        UPDATE accounts SET closed = 1 WHERE id IN (SELECT id from tempAccountIds);
        SELECT * FROM accounts WHERE id IN (SELECT id from tempAccountIds);
        DROP TABLE tempAccountIds;
    END|
DELIMITER ;