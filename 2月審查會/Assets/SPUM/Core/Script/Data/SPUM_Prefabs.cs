using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerState
{
    IDLE,
    MOVE,
    DAMAGED,
    DEBUFF,
    DEATH,
    OTHER
}

public class SPUM_Prefabs : MonoBehaviour
{
    public Animator _anim;

    [Header("Animation Lists")]
    public List<AnimationClip> IDLE_List = new();
    public List<AnimationClip> MOVE_List = new();
    public List<AnimationClip> DAMAGED_List = new();
    public List<AnimationClip> DEBUFF_List = new();
    public List<AnimationClip> DEATH_List = new();
    public List<AnimationClip> OTHER_List = new();

    public Dictionary<PlayerState, List<AnimationClip>> StateAnimationPairs =
        new Dictionary<PlayerState, List<AnimationClip>>();

    void Awake()
    {
        if (_anim == null)
            _anim = GetComponent<Animator>();
    }

    // ✅ 給 PlayerObj.cs 用的（關鍵）
    public bool allListsHaveItemsExist()
    {
        List<List<AnimationClip>> allLists = new()
        {
            IDLE_List,
            MOVE_List,
            DAMAGED_List,
            DEBUFF_List,
            DEATH_List,
            OTHER_List
        };

        return allLists.All(list => list != null && list.Count > 0);
    }

    public void PopulateAnimationLists()
    {
        StateAnimationPairs.Clear();

        StateAnimationPairs[PlayerState.IDLE] = IDLE_List;
        StateAnimationPairs[PlayerState.MOVE] = MOVE_List;
        StateAnimationPairs[PlayerState.DAMAGED] = DAMAGED_List;
        StateAnimationPairs[PlayerState.DEBUFF] = DEBUFF_List;
        StateAnimationPairs[PlayerState.DEATH] = DEATH_List;
        StateAnimationPairs[PlayerState.OTHER] = OTHER_List;
    }

    public void OverrideControllerInit()
    {
        if (_anim == null) return;
        _anim.Rebind();
        _anim.Update(0f);
    }

    public void PlayAnimation(PlayerState state, int index = 0)
    {
        if (!StateAnimationPairs.ContainsKey(state)) return;

        var list = StateAnimationPairs[state];
        if (list == null || list.Count == 0) return;
        if (index < 0 || index >= list.Count) index = 0;

        _anim.Play(list[index].name);
    }
}
