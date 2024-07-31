/* ##############################
 * #  Narrenschlag's DialogueZ  #
 * # Professional Dialogue Tool #
 * #         for  Unity         #
 * ##############################
 */

#region Documentation
/*
* Basics of understanding
* This system works by using a database that is called by the DialogueZ Manager
* It generates an easy and fast to gather from dictionary from the database
* The ui can be set to how ever you want. Customize as you wish! :)
*/

#region !!!KEY WORDS TO KNOW!!!
/*
 * ids = the string id that you can assign to elements of the database and events
 * 
 * idi = the little int that you can see at the right of the ids
 */
#endregion

#region Usage short tutorial
/*
* Init with custom database
* Just use DialogueZ.Init as usuall but with a database as second argument
* 
* Quit dialogue via message command
* Just write "//quit//"
* 
* Quit by script
* Use DialogueZStyle.singleton.Quit()
* 
* Instant finish typing
* Use DialogueZStyle.singleton.done_typing = true
*/
#endregion

#region message command examples
/*
 * -> Commands
 * "//call, print_hello//"  -> calls event in Manager with ids="print_hello"
 * "//get, playername//"    -> return the value read from the style component with the id="playername"
 * "//quit//"               -> will close the dialogue window on next follow or message init
 * 
 * -> Requirements
 * -> "*" stands for "get from style.Read()"
 * "//mark, =, *playername" -> return bool => Style.Read("playername") is "mark"
 * "//mark, =, playername"  -> return bool => "playername" is "mark"
 * 
 * "//*level, >, 2"         -> return bool => Style.Read("level")   is greater  "mark"
 * "//3, =, 2"              -> return bool => 3                     is equal    2
 */
#endregion

#region typing
/*
 * Set the typing speed to 0 to set the mode to instant text
 * else it will type in the defined speeed
 * 
 * auto finish it by setting 
 * DialogueZStyle.singleton.done_typing = true
 */
#endregion

#region Manager
/*
 * The "Manager" is nothing else than the DialogueZ.cs component attached to a gameObject
 * 
 * It contains handles the callable commands and their events
 * Just change them in the inspector!
 * 
 */
#endregion

#region Scaler Component
/*
 * The DialogueZScaler component is used to easily and fast auto scale stuff
 * like the follow root, the title box or the message box
 * 
 * You can take a look at the samples in the demo scene to learn how it works
 * and test out to make it fit your vision
 * 
 * Remember to set the fitting type
 */
#endregion
#endregion