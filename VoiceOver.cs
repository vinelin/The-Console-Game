using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    class VoiceOver
    {
        public enum Situation
        {
            begin1,
            begin2,
            knock1,
            knock2,
            knock3_1,
            knock3_2,
            gg,
            fight,
            victory,
            fail,
            enterlibrary,
            finishlibrary,
            knockwithfo,
            enterFoEnd,
            FoEndVictory,
            FoEndFail,
            BestEnd,
            wantToBack
        }
        static Dictionary<Situation, string> WhatToSpeak = new Dictionary<Situation, string> {
            { Situation.begin1,"你死了，但没有完全死。鉴于你的上一世太没存在感，天堂地狱都不想接收你。现在交给你一个讨伐大魔王的任务，完成后可以回到你之前的世界的小时候，让你保留记忆的重开。"},
            { Situation.begin2,"当然你也没得选，左边有佛像，右边有一座图书馆，穿过最右边的小径就是魔王的居所。去自己探索如何打败魔王吧！对了，千万别往下走！"},
            { Situation.knock1,"你对着佛像磕了一个头，好像什么都没有发生。"},
            { Situation.knock2,"别磕了，这里什么都没有，去先完成别的任务吧。"},
            { Situation.knock3_1,"你是真无聊！磕了一百个头后之后，把地板撞破了，发现一本书。书上若影若现四个字《龟派气功》。获得《龟派气功》和头破血流。"},
            { Situation.knock3_2,"你现在无比强大，去挑战魔王吧！"},
             { Situation.gg,"啥也不会，现在去只能送人头！"},
             { Situation.wantToBack,"没有回头路，继续前行吧！"},
             { Situation.enterlibrary,"把散落的书放到箱子里可能有意想不到的收获哦。"},
             { Situation.finishlibrary,"卧槽搬书搬出了佛教秘典，对佛教感悟更深了。现在赶快去卍那里看看。"},
             { Situation.knockwithfo,"你对佛教感悟让你获得了圣光的庇护！可以对BOSS发起挑战了！"},
              { Situation.fight,"按空格使用龟派气功和BOSS对波吧！"},
              { Situation.enterFoEnd,"魔王见你被圣光环绕，知道打不过你，对你进行智取！    你只有一次机会选择你的答案。    提示：恶魔不喜欢一句话说两遍。"},
               { Situation.FoEndVictory,"你醒了，发现一切只是你做的一个梦。                              True Ending."},
               { Situation.FoEndFail,"你的智力没有重开的价值。           Bad Ending."},
              { Situation.victory,"牛逼啊，你把魔王打败了！我送你回去!          Good Ending ??"},
              { Situation.fail,"你失败了！你将从任何世界消失。             Bad Ending."},
              { Situation.BestEnd,"原来你才是本世界的主宰。               Best Ending."},




        };

        public static void ClearDialog()
        {
            for (int top = 9; top <= 14; top++)
            {
                for (int left = 34; left <= 60; left += 2)
                {
                    Console.SetCursorPosition(left, top);
                    Console.Write("　");
                }
            }
        }

        public static void Speak(Situation now)
        {

            ClearDialog();
            int top = 9;
            int left = 34;
            for(int i = 0; i < WhatToSpeak[now].Length; i++)
            {
                Console.SetCursorPosition(left, top);
                Console.Write(WhatToSpeak[now][i]);
                if (left == 60)
                {
                    top++;
                    left = 34;
                    continue;
                }
                left += 2;
            }


        }



    }
}
