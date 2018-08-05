# ScriptSDK.SantiagoUO.RaillessMining

## Configuration

### Windows Registry

#### Required
| --- | --- | --- | --- | --- |
| Key | Sub key | Description | Type | Default |
| Software\ScriptSDK.SantiagoUO\RaillessMining\[CharacterName] | DropContainerId | ID (EasyUO) of the drop container | string | - |

#### Overrides
| --- | --- | --- | --- | --- |
| Key | Sub key | Description | Type | Default |
| Software\ScriptSDK.SantiagoUO\RaillessMining\[CharacterName] | SmeltWeight | Smelt ore once above this weight | int | 350 |
| Software\ScriptSDK.SantiagoUO\RaillessMining\[CharacterName] | Smelt1x1 | Split ore in stack of 1 before smelting | bool | false |
| Software\ScriptSDK.SantiagoUO\RaillessMining\[CharacterName] | BankWeight | Bank ores/ingots once above this weight after smelting | int | 100 |
| Software\ScriptSDK.SantiagoUO\RaillessMining\[CharacterName] | PickaxesCount | Amount of pickaxes to have before returning to mine | int | 2 |