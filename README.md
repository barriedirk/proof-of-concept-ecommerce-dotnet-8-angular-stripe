# Proof or concept of an e-commerce with dotnet 8, angular and using stripe

This is a course I did to practice, if you desire to see the course, this is the [course](https://www.udemy.com/course/learn-to-build-an-e-commerce-app-with-net-core-and-angular).

# Running the project

You can run this app locally.
The requirements are:

1. Docker 8
2. .Net SDK v8
3. NodeJS (at least version 20.11.1) 


To restore the packages by running:

```bash
# From the solution folder (skinet-2024)
dotnet restore

# Change directory to client to run the npm install.  Only necessary if you want to run
# the angular app in development mode using the angular dev server
cd client
npm install
```

If you wish to see the payment functionality working with stripe you will need to create a Stripe account and populate the keys from Stripe.  In the API folder create a file called ‘appsettings.json’ with the following code:

```json
{
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "StripeSettings": {
      "PublishableKey": "pk_test_REPLACEME",
      "SecretKey": "sk_test_REPLACEME",
      "WhSecret": "whsec_REPLACEME"
    },
    "AllowedHosts": "*"
  }
```
To use the Stripe webhook you will also need to use the StripeCLI, and when you run this you will be given a whsec key which you will need to add to the appsettings.json.   To get this key and instructions on how to install the Stripe CLI you can go to your Stripe dashboad ⇒ Developers ⇒ Webhooks ⇒ Add local listener.   The whsec key will be visible in the terminal when you run Stripe.

Once you have the Stripe CLI you can then run this so it listens to stripe events and forward them to the .Net API:

```bash
#login to stripe
stripe login

# listen to stripe events and forward them to the API
stripe listen --forward-to https://localhost:5001/api/payments/webhook -e payment_intent.succeeded
```

This app uses both Sql Server and Redis. Sql Server to store the products, orders, accounts and redis for cache and the cart.

There is a docker compose file with this configuration and volumes. These are both configured to run on their default ports so ensure you do not have a conflicting DB server running on either port 1433 or port 6379 on your computer:

```bash
docker compose up -d 
```

You can then run the app and browse to it locally by running:

```bash
# run this from the API folder
cd API
dotnet run
```

You can then browse to the app on https://localhost:5001

If you wish to run the Angular app in development mode you will need to install a self signed SSL certificate.  The client app is using an SSL certificate generated by mkcert.   To allow this to work on your computer you will need to first install mkcert using the instructions provided in its repo [here](https://github.com/FiloSottile/mkcert), then run the following command:

```bash
# cd into the client ssl folder
cd client/ssl
mkcert localhost
```

You can then run both the .Net app and the client app.

```bash
# terminal tab 1
cd API
dotnet run

# terminal tab 2
ng serve
```

Then browse to [https://localhost:4200](https://localhost:4200) 

The Stripe test cards available from [here](https://docs.stripe.com/testing#cards) to pay for the orders.

Note:
If you review this code, you will notice that there are some files with the txt extension. It's because during the course, the code was changed and I didn't want to lose the previous changes to do a comparison.