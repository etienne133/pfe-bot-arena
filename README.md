# pfe-bot-arena
pfe bot arena 

## Startup (what I use)
start python env from main folder:
..\PythonEnv\BattleBotsVenv\Scripts\Activate

## Start listening without curriculum port:
mlagents-learn config\battlebots.yaml --run-id battlebots_01 --train
mlagents-learn config\battlebots.yaml --run-id battlebots_4player_01 --train

## Open Tensorboard:
tensorboard --logdir=results

## maybe helpful Commands:
These commands a dependent on your OS and where the files are located. Make sure to read carefully and adjust accordingly. 

mlagents-learn config/trainer_config.yaml --curriculum config/curricula/penguin.yaml --run-id penguin_01 --train

CD into Folder where Repo is located: cd {FolderContainingRepo}
CD into Repo: cd A.I.\ from\ Scratch\ -\ ML\ Agents\ Example
CD into TrainerConfig: cd TrainerConfig
First Run: mlagents-learn trainer_config.yaml --run-id="JumperAI_1"
Open Tensorboard: tensorboard --logdir=results
Second Run: mlagents-learn trainer_config.yaml --run-id="JumperAI_2"
Last Run: mlagents-learn trainer_config.yaml --run-id=JumperAI_3 --env=../Build/build.app --time-scale=10 --quality-level=0 --width=512 --height=512