# pfe-bot-arena
pfe bot arena 

## Startup 
todo 


## Commands:
These commands a dependent on your OS and where the files are located. Make sure to read carefully and adjust accordingly. 

CD into Folder where Repo is located: cd {FolderContainingRepo}
CD into Repo: cd A.I.\ from\ Scratch\ -\ ML\ Agents\ Example
CD into TrainerConfig: cd TrainerConfig
First Run: mlagents-learn trainer_config.yaml --run-id="JumperAI_1"
Open Tensorboard: tensorboard --logdir=summaries
Second Run: mlagents-learn trainer_config.yaml --run-id="JumperAI_2"
Last Run: mlagents-learn trainer_config.yaml --run-id=JumperAI_3 --env=../Build/build.app --time-scale=10 --quality-level=0 --width=512 --height=512