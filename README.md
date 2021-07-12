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

## Calling your command
All commands are called using a `CommandContext`, which represents enviroment for commands to be called and handles the parsing and execution of commands. 
Commander uses a case *insenstive* syntax similar to that of the windows command prompt, with the command name and then the arguments separated by spaces 
(Ex. `name arg0 arg1 arg2`). If there are no arguments then submitting just the name of the command invokes it.

To call a command, use the `CommandContext.SubmitCommand(string)` or `Command.TrySubmitCommand(string)` methods:

```C#
static void Main(string[] args)
{
    var context = new CommandContext();
    context.SubmitCommand("print hi"); // outputs "hi" to the context's output
}
```


You can find more information about Commander [here](https://github.com/Redninja106/Commander/wiki).
