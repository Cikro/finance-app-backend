# Endpoints

- [Endpoints](#endpoints)
  - [Overview](#overview)

## Overview

A list of all the available routes.
Note that "DELETE" is not really supported. You never want to delete financial information, only document changes..... // THIS LINE NEEDS CHANGES
//NOTE: Need to decide between PATCH vs POST.

| Routes                                           | GET  | POST | PUT | DELETE | Notes                                                              |
|--------------------------------------------------|------|------|-----|--------|--------------------------------------------------------------------|
| /accounts                                        |  X   |   X  |     |        | A collection of accounts. Post to add a new account.                |
| /accounts/:id                                    |  X   |   X  |     |    X   | Get/Update/Delete a specific account.                              |
| /accounts/:id/transactions                       |  X   |      |     |        | Get a collection of transactions on an account.                    |
| /accounts/:id/transactions/:id                   |  X   |      |     |        | Get a specific transaction on an account.                          |
| /accounts/:id/transactions/:id/journal-entry     |  X   |   X  |     |        | Get/Update the journal entry a transaction is a part of.           |
| /journal-entries                                 |  X   |   X  |     |        | A collection of of journal entries.                                |
| /journal-entries/:id                             |  X   |      |  X  |        | Get/update a specific journal entry.                               |

