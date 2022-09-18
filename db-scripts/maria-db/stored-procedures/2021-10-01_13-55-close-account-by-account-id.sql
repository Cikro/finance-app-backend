DELIMITER |
DROP PROCEDURE IF EXISTS CloseAccountByAccountId|
CREATE PROCEDURE
    CloseAccountByAccountId(accountId int UNSIGNED)
    MODIFIES SQL DATA
    begin
       CREATE TEMPORARY TABLE tempAccountIds WITH recursive allRelatedAccounts AS (
            SELECT * FROM accounts 
            WHERE parent_account = accountId and closed = false
            UNION DISTINCT
            SELECT a.* 
            FROM accounts AS a, allRelatedAccounts AS r
            WHERE
                (
                    a.closed = false 
                    and 
                    (
                        a.id = r.parent_account
                        OR a.parent_account = r.id
                    )
                ) 
        ) select id from allRelatedAccounts WHERE closed = false;
        
         UPDATE accounts SET closed = 1 WHERE id IN (SELECT id from tempAccountIds);
        
        SELECT * FROM accounts WHERE id IN (SELECT id from tempAccountIds);
        DROP TABLE tempAccountIds;
        
    END|
DELIMITER ;