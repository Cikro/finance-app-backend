### Closing Accounts
Having an endpoint `DELETE /accounts` to close an account can be kind of weird witha parent-child relationship.
    * Does it close all of the children on the account? What about the children's children?
      * This is the appre
    * Does it prevent all closing until all of the children on the account are closed? 
      * This way would be weird, since you would need to post to each account such to close each one individually. 
        Or call and endpoint such as `DELETE /accounts/children`. Would that endpoint require each child account to be closed too?
        Logically yes, otherwise it would behave differently than `DELETE /accounts`. 
      * This would be a pain in the butt to use  
    * Conclusion:
      * Have `DELETE /accounts` close all accounts and all of it's children
      * I'm Not actually sure what depth of parent-child  relationship I would need / use. It may be better to just assume 1 level
        for now, and then if things come up, try adding more.  