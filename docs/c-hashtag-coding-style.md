
# C hashtag coding style

## Braces

Braces go on the same line

##### DO

```cs
namespace {
    class MyClass {
        private myMethod() {
            var myLocal = new MyStruct {
                valueA = 5,
                valueB = 5,
            };
            if (myValue) {
                for (int i = 0; i < 0; i++) {

                }
            } else {
                while (myValue) {

                }
            }
        }
    }
}
```

##### DONT

```cs
namespace MyNamespace
{
    class MyClass
    {
        private myMethod()
        {
            var myLocal = new MyStruct
            {
                valueA = 5,
                valueB = 5,
            };
            if (myValue)
            {
                for (int i = 0; i < 0; i++)
                {

                }
            }
            else
            {
                while (myValue)
                {

                }
            }
        }
    }
}
```

## Naming

All values are `camelCase`, all types are `PascalCase`. 

`enum`-values are considered types and should be `PascalCase`

##### DO

```cs
namespace MyNamespace {
    enum MyEnum {
        OptionOne,
        OptionTwo,
        OptionThree,
    }
    class MyClass {
        private string myField = "";
        private static const int myStaticField = 123;

        private myMethod() {
            var myVariable = myValue;
        }

        public static myStaticMethod() {}
    }
}
```

##### DONT

```cs
namespace my_Namespace {
    enum My_enum {
        optionOne,
        OPTION_TWO,
        option_three,
    }
    class myClass {
        private string MyField = "";
        private static const int MY_STATIC_FIELD = 123;

        private my_method() {
            var i_my_variable = myValue;
        }

        public static MyStaticMethod() {}
    }
}
```
