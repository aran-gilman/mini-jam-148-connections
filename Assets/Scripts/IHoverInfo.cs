// To add hover info to a game object:
//  1) Create a new child object.
//  2) Add a Collider2D (e.g. BoxCollider2D).
//  3) Add a component deriving from IHoverInfo.
//  4) Set the object's layer to HoverInfo.
public interface IHoverInfo
{
    string DisplayName { get; }
    string Description { get; }
}
