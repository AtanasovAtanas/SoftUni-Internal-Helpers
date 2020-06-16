[slide]

# Race

[code-task title="Race" taskId="java-fundamentals-regex-2" executionType="tests-execution" executionStrategy="html-and-css-zip-file" requiresInput="false"]

[code-upload allowedMemory="30" /]

[task-description]

## Description

Write a program that processes information about a race.

On the first line you will be given a list of participants separated by ", ".

On the next few lines until you receive a line "end of race" you will be given some info which will be some alphanumeric characters.

In between them you could have some extra characters which you should ignore.

For example: "G!32e%o7r#32g\$235@!2e".

The letters are the name of the person and the sum of the digits is the distance he ran.

So here we have George who ran 29 km.

Store the information about the person only if the list of racers contains the name of the person.

If you receive the same person more than once just add the distance to his old distance.

At the end print the top 3 racers ordered by distance in descending in the format:

## Examples



[/task-description]

[tests]
[test open]
[input]
Welcome to SoftUni and have fun learning programming
[/input]
[output]
learning
Welcome
SoftUni
and
fun
programming
have
to
[/output]
[/test]
[test]
[input]
a b
[/input]
[output]
b
a
[/output]
[/test]
[test]
[input]
PHP Java C\#
[/input]
[output]
Java
PHP
C\#
[/output]
[/test]
[test]
[input]
10S 7H 9C 9D JS
[/input]
[output]
7H
JS
10S
9C
9D
[/output]
[/test]
[test]
[input]
hello
[/input]
[output]
hello
[/output]
[/test]
[test]
[input]
pesho gosho kiro miro niki pipi koko
[/input]
[output]
kiro
gosho
koko
miro
niki
pesho
pipi
[/output]
[/test]
[/tests]
[/code-task]
[/slide]
