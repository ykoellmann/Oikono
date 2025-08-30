﻿using Mono.Cecil;
using Mono.Cecil.Rocks;
using NetArchTest.Rules;

namespace Oikono.UnitTests.Rules;

public class AsyncMethodsHaveSuffixAsyncRule : ICustomRule
{
    public bool MeetsRule(TypeDefinition type)
    {
        return type
            .GetMethods()
            .All(method =>
                (method.CustomAttributes.Any(type => type.AttributeType.Name.Equals("AsyncStateMachineAttribute"))
                 && method.Name.EndsWith("Async"))
                || (!method.CustomAttributes.Any(type => type.AttributeType.Name.Equals("AsyncStateMachineAttribute"))
                    && !method.Name.EndsWith("Async")));
    }
}