# Commander

Commander is a quick and easy command-running engine inspired by powershell. 

# Getting started

## Writing a command

A command is simply a static method with an attribute:

```C#

[Command]
public static void MyCommand()
{
    // Do something here
}

```

## Calling a command
Commander uses a case *insenstive* syntax similar to that of the windows command prompt, with the command name and then the arguments separated by spaces 
(Ex. `name arg0 arg1 arg2`). If there are no arguments then submitting just the name of the command invokes it.

To call a command, use the `Service.SubmitCommandString()` method:

```C#
static void Main(string[] args)
{
    Service.SubmitCommandString("MyCommand");
}
```

# Commander concepts
## Services
A "service" is like a namespace for commands. By default, a new command is placed into a service with a name the same as it's declaring type,
but you can declare it otherwise using the `Command` attribute:
```C#
[Command("MyService")] 
public static void MyCommand()
{
    // Do something here
}
```
To call a command in a specific service, use the `service:name` syntax:
```
MyService:MyCommand
```
This is required if there multiple commands of the same name in different services.

## Command Arguments
Adding arguments to your command is as simple as adding parameters to your command method:

> Note: arguments *are* case sensitive. 
```C#
[Command("MyService")] 
public static void MyCommand(string myArg0, int myArg1, decimal myArg2)
{
    // Do something with myArg0, myArg1, and myArg2 here
}
```
Arguments can be strings or any primitive type.


## Optional Arguments
Commander does not support overloading, but does support optional arguments. For example, take the builtin `say` command:
```C#
[Command]
public static void Say(string words, int times = 1)
{
    for(int i = 0; i < times; i++)
        Console.WriteLine(words);
}
```

Since times is an optional parameter, `say "hello world"` and `say "hello world" 1` both run without errors, and the same output.

## Command Documentation

Commander uses attribuutes for command documentation, along with the `doc` command. 
Calling `doc` and passing any command name will print a nicely formatted description of the command, it's parameters, and how to use it.

### Writing Command Documentation

To write documenation for your command, you need to use attributes. Take this command:
```C#
[Command]
public static void Add(int a, int b)
{
    Console.WriteLine(a + b);
}
```

To add a description of the command, use the `Doc` attribute:

```C#
[Command]
[Doc("Adds two numbers together")]
public static void Add(int a, int b)
{
    Console.WriteLine(a + b);
}
```
To add information about arguments, use the `DocArg` attribute:
```C#
[Command("Adder")]
[Doc("Adds two numbers together")]
[DocArg("int", "a", "The first number to add")]
[DocArg("int", "b", "The second number to add")]
public static void Add(int a, int b)
{
    Console.WriteLine(a + b);
}
```
Calling `doc add` results in the following output:
```
+=========================================== Program:Add ===========================================+

        Description: Adds two numbers together

        Parameters:
                "a" (int): The first number to add
                "b" (int): The second number to add

        Usage: Adder:Add a b

+===================================================================================================+
```
