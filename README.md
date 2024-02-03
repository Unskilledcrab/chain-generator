# chain-generator

## Overview

This is a simple application that uses the OpenAI GPT-3 API to generate a chain of text based on a given input. The application is built using .NET 8 and uses the OpenAI GPT-3 API to generate the chain of text.

## Configuration

You'll need to add your OpenAI API key in order to test the application.

You'll need to first go to https://platform.openai.com/api-keys and create an API key.

Next you'll need to take your API key and update your local secrets.json file by going to the `Solution Explorer` and right clicking on the `ChainGenerator` project and selecting `Managed User Secrets` and the following:

```json
{
  "OpenAIServiceOptions:ApiKey": "YOUR_API_KEY"
}
```
