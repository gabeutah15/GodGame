using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal
{
    public Dictionary<string, int> sgoals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}

public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;


    // Start is called before the first frame update
    public void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
        {
            actions.Add(a);
        }
    }

    bool invoked = false;

    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(currentAction != null && currentAction.running)
        {
            if(currentAction.agent.hasPath && currentAction.agent.remainingDistance < 1f)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        if(planner == null || actionQueue == null)
        {
            planner = new GPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;//sort goals by importance

            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.plan(actions, sg.Key.sgoals, null);
                if(actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if(actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }

        if(actionQueue != null && actionQueue.Count > 0)//still have actions left in queue
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                //if the action does not have a target but does have a tag, find a game object with that tag and assign it
                // as the new action
                //if(currentAction.target == null && currentAction.targetTag != "")
                //    currentAction.target = GameObject.FindGameObjectWithTag(currentAction.targetTag);

                //if the tag is not null then set destination to that target, but shouldnt it be if target is not null?
                if(currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }
}
