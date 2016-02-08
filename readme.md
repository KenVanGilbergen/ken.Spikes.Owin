# ken.Spikes.Owin
---

A bunch of middleware that can be used to teach the owin specification.

## Middleware

* UseDebugMiddleware();
* UseHtmlAppenderMiddleware();
* UseCornifyMiddleware();
* UseKonamiCodeMiddleware();
* UseServeDirectoryMiddleware();

## Things to remember

* Once you write to the body you can no longer change the headers
* Set the content-type correctly

## Humans

Projects I am stealing from...

* [https://github.com/Cornify/Cornify](https://github.com/Cornify/Cornify)
* [https://github.com/snaptortoise/konami-js](https://github.com/snaptortoise/konami-js)
