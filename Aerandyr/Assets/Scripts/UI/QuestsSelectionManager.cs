using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OneManEscapePlan.UIList.Scripts;
using UnityEngine;

[RequireComponent(typeof(QuestsController))]
public class QuestsSelectionManager : UIListSelectionManagerBase<Quest, QuestView>
{
}
