# Finance App Server

## Table of Contents

  1. [Introduction](#introduction)
  2. [Setup and Running](#setup-and-running)
  3. [Design](#design)
  4. [Choices of Technology](#choices-of-technology)

## Introduction
I needed something to replace my excel spreadsheet that I use to keep track of my income, spending and budget. I figure I can use this as an excuse to 
learn about full-stack web development. 

This server will provide at a minimum a RESTful API for an accrual accounting-like system where users can debit and credit Asset, Liability, Expense and Income accounts that they created. Since I am planning to host this for myself and family, I plan to implement two-way SSL authentication on a reverse proxy server for added security.

## Setup and Running
### MariaDB
#### Installation and Setup
  Follow the instructions for "Installation", "Initial Setup", and "User Creation and Database Creation" here: https://fedoraproject.org/wiki/MariaDB

## Design
  See /design files for Database tables and ~~REST API design~~

## Choices of Technology

### Dotnet Core
Since I know some ES6 JavaScript I initially I had planned to use Node Express for this server. There appears to be as more jobs in my area using other frameworks.
Supported by Microsoft, I chose Dotnet Core. Dot Net has many useful standard libraries which may be beneficial as the project grows.

### ReactJS
I've have some experience working with the Angular2 framework. There are more jobs in my area which require React experience. This a good opportunity to learn a new 
technology.

### NGINX
It's apparently very common to use NGINX as a reverse proxy. It would be a good place to implement SSL authentication to decouple it from the actual program logic.
Using a reverse proxy will also allow for the hosting of multiple web services from the same server.

### Docker
I don't know if I will use Docker. It will probably help with deployment and would help the application scale (which I probably won't need)

