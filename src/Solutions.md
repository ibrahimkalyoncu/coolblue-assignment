## Task 1 [BUGFIX]:

##### Requirement : 

The financial manager reported that when customers buy a laptop that costs less than € 500, insurance is not calculated, while it should be € 500.


##### Issue :

The issue was, product type specific insurance logic was encapsulated by else block. So it was not applying for the ones lower than 500 EUR.


##### Solution :

Moved the if block out of else to make it work after all the logic and solved. Did not refactor anything on this commit as it was a separate task.

## Task 2 [REFACTORING]:

##### Requirement : 

It looks like the already implemented functionality has some quality issues. Refactor that code, but be sure to maintain the same behavior.

##### Issues :

- Instead of decoupling services, they used highly coupled each other (BusinessRules inside HomeController) (It makes it hard to write tests)
- Request and response models are better to be different even they are almost have the same fields.
- As long as there is no write operation and it's just a read, then HttpGet should be preferred. Also rooting definition is better to match controller name
- ref and out usages should be avoided as much as possible.
- Usage of many if-else trees should be avoided
- Each class definition should be on it's own file. Also folder structure should make sense. 
- String comparisons should be avoided
- Configurations should be placed on config files or environment variables
- **Never frequently initiate a HttpClient**. Port exhaustion issue may kill you
- **Never use dynamic type**. C# is type-safe. Keep it safe.
- Avoid code repeating. (BusinessRules.cs)
- When possible, use async-tasks for better managed concurrency out of box.

##### Solution :

- Implemented a ProductApiClient. Will be reused on next tasks. (OpenAPI seems also available but not used to it so much so I didn't prefer it in thi case)
- Converted hardcoded insurance logic into a configurable data.
- Implemented an insurance calculator based on the new configurable data. Easy to test now. 
- Replaced the usage of existing code with new approach.
