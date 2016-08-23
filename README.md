# Word2vec.Tools#
.Net Implementation for those who wants to use google word2vec tools in theirs .net solutions

To install Word2Vec .Net Tools, run the following command in the Package Manager Console:

```
PM> Install-Package Word2vec.Tools 
```

## What is the word2vec?

word2vec is great Natural language processing technology mades by google. 
It presents each word as N-dimensional vector, so you can do any math operations with that like distance, substraction, addition, average and so on.

Examples: 
- "France" relates to "Paris" as "Russia" relates to .... "Moscow"  (France - Paris + Russia => Moscow)
- "Boy" + "girl" = ...."baby"

## What can "Word2vec.Tools" do?

It can
- Get vectors representation of words
- Calculate words proximity
- Search words analogies
- words Substraction and additions
- Make it in clean OO-style

## What do i need for start?

You need to generate vectors.bin or vectors.txt sampling file. 

Usualy people downloads large (about 10GB and more) text file (Wikipedia dump is good for that) and generate theirs own samplings files
Watch more on https://code.google.com/archive/p/word2vec/

## Ok, i've got vectors bin file. Is it hard to start using your tools? 

Not at all. Two or three lines of code, depends on your task. See Examples project for your easy-start

## Why don't you implement your own vectors.bin generator? Are you lazy?

Actualy - yes. That's why i'am software engineer.
On other side i can not to do that better than google have done. They have built perfect and fast c-tools for that. 
You definitely have to use it.





