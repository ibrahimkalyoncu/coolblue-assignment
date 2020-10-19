## Task 1 [BUGFIX]:

##### Requirement : 

The financial manager reported that when customers buy a laptop that costs less than € 500, insurance is not calculated, while it should be € 500.


##### Issue :

The issue was, product type specific insurance logic was encapsulated by else block. So it was not applying for the ones lower than 500 EUR.


##### Solution :

Moved the if block out of else to make it work after all the logic and solved. Did not refactor anything on this commit as it was a separate task.

