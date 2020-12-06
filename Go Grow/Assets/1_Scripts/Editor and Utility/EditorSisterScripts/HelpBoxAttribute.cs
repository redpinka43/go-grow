using UnityEngine;
 
/* Found on this page:
https://forum.unity.com/threads/helpattribute-allows-you-to-use-helpbox-in-the-unity-inspector-window.462768/
 */

public enum HelpBoxMessageType { None, Info, Warning, Error }
 
/// <summary>
/// Creates a read-only text box in the inspector. Can assign types of None,
/// Info, Warning, and Error.
/// 
/// <code>
/// Example --------
/// <code>[HelpBox("This is some help text for Data.", HelpBoxMessageType.Info)]</code>
/// <code>public string data;</code>
/// </code>
/// </summary>    
public class HelpBoxAttribute : PropertyAttribute {
 
    public string text;
    public HelpBoxMessageType messageType;
 
    public HelpBoxAttribute(string text, HelpBoxMessageType messageType = HelpBoxMessageType.None) {
        this.text = text;
        this.messageType = messageType;
    }
}