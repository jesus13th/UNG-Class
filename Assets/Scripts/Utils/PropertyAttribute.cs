using UnityEngine;

public class ShowIfAttribute : PropertyAttribute {
    public string ConditionFieldName { get; private set; }

    public ShowIfAttribute(string conditionFieldName) {
        ConditionFieldName = conditionFieldName;
    }
}