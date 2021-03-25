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

You can find more information about Commander [here](https://github.com/Redninja106/Commander/wiki).
