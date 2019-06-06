# Finance App Server

## Table of Contents

  1. [Introduction](#introduction)
  2. ~~[Setup and Running](#setup-and-running)~~
  3. ~~[Design](#design)~~
  4. [Choices of Technology](#choices-of-technology)

## Introduction
I needed something to replace my excel spreadsheet that I use to keep track of my income, spending and budget. I figure I can use this as an excuse to 
learn about full-stack web development. 

This server will provide at a minimum a RESTful API for an accrual accounting-like system where users can debit and credit Asset, Liability, Expense and Income accounts that they created. Since I am planning to host this for myself and family, I plan to implement two-way SSL authentication on a reverse proxy server for added security.

## ~~Setup and Running~~


## ~~Design~~
~~See /design files for Database tables and REST API design~~

## Choices of Technology

### Node Express

I have a little bit of experience with ES6 JavaScript from a previous job and I want to create a front-end for this application sometime in the future. Node uses JavaScript. Not having to learn the syntax of another programming language will allow me to focus more on learning about server-side programming.

JavaScript uses a single threaded event-loop and is not suited to do large computations. Since this will be a RESTful API which will deal with storing user input in a database and retrieving it, this should not be an issue for this application.

### NGINX
It's apparently very common to use NGINX as a reverse proxy. It would be a good place to implement SSL authentication to decouple it from the actual program logic.
Using a reverse proxy will also allow for the hosting of multiple web services from the same server.

### Docker
I don't know if I will use Docker. It will probably help with deployment and would help the application scale (which I probably won't need)
