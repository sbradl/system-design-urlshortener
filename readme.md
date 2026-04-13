# System Design for URL Shortener

## Introduction

This repository is my attempt to learn something about system design
as well as containerization.

Containers are used to build, test and run multiple components
of a url shortener as shown in
[this awesome video on the System Design Fight Club YouTube Channel](https://www.youtube.com/watch?v=tm-SWO9gUAU).

## Setup

Following software needs to be installed:

- docker
- docker buildx
- docker compose
- optional: [Dive](https://github.com/wagoodman/dive) - a very handy tool to look into docker images to see their contents

## Build and Run

Run ```docker compose up``` in the main directory.

## Design

### Frontend

Angular application for the user who wants to create a shortened url.
Just some text, an input and a button.
Why Angular? Because I know it and wanted to play with the latest version.

### Shortener Service

Dotnet WepApi

### Broker

### Data store

### Cache

### Redirector Service

Dotnet WepApi
