## How to run:

All services are dockerized. Simply run the **run.bat** to get all the services (insurance.api, productData.api, mssql server, elasticseach and kibana) up and running. Also if all goes well you should have the initial test data stored inside the database.

## Task 1 [BUGFIX]:

### Requirement : 

The financial manager reported that when customers buy a laptop that costs less than € 500, insurance is not calculated, while it should be € 500.


### Issue :

The issue was, product type specific insurance logic was encapsulated by else block. So it was not applying for the ones lower than 500 EUR.


### Solution :

Moved the if block out of else to make it work after all the logic and solved. Did not refactor anything on this commit as it was a separate task.

## Task 2 [REFACTORING]:

### Requirement : 

It looks like the already implemented functionality has some quality issues. Refactor that code, but be sure to maintain the same behavior.

### Issues :

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

### Solution :

- Implemented a ProductApiClient. Will be reused on next tasks. (OpenAPI seems also available but not used to it so much so I didn't prefer it in thi case)
- Converted hardcoded insurance logic into a configurable data.
- Implemented an insurance calculator based on the new configurable data. Easy to test now. 
- Replaced the usage of existing code with new approach.

## Task 3 [FEATURE 1]:

### Requirement : 

Now we want to calculate the insurance cost for an order and for this, we are going to provide all the products that are in a shopping cart.

### Solution :

This is similar to product, but this time there may be many products and also quantity is an other parameter effects the insurance cost.

I implemented a new endpoint having a simple order model with product id and quantity for each order item. Then calculated the cost based on the rules.

## Task 4 [FEATURE 2]:

### Requirement : 

We want to change the logic around the insurance calculation. We received a report from our business analysts that Drones are getting lost more than usual. Therefore, if an order has one or more drones, add € 500 to the insured value of the order.

### Solution :

This is so similar to the Laptop and Smartphones requirement but this time applied once in an order and not included in product insurance cost. 

So basically we have 2 types of insurance rules;
- By price range
- By product type

Also, for the by product type rules, we have two different sub type
- Applies to product
- Applies to order

With these assumptions I extended the generic solution to cover all these cases.  

## Task 5 [FEATURE 3]:

### Requirement : 

As a part of this story we need to provide the administrators/back office staff with a new endpoint that will allow them to upload surcharge rates per product type. This surcharge will then need to be added to the overall insurance value for the product type.

Please be aware that this endpoint is going to be used simultaneously by multiple users.

### Solution :

As there is a possibility of setting a surcharge rate for the same product type simultaneously, I implemented it as UPSERT.

Still, there may be race condition issues. This can be prevented by;

- Using an application level lock. This will only prevent running apps on single instance as it's a thread base solution. Distributed applications will still have issue
- Using a DB level lock (simply using transaction isolation level as SERIALIZABLE). But this may bring some performance issues. Actually it's just a single insert/update so shouldn't be much issue but I would not prefer to affect customer related endpoints performance for some so rare back office issues.

   