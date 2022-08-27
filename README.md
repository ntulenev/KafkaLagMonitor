# KafkaLagMonitor
Kafka lag monitoring utility (initial version)

Based on discussion on https://github.com/confluentinc/confluent-kafka-dotnet/issues/748

Output example:
```
Lag for groupId = commands
 --------------------------------------------------------
 | Topic                              | Partition | Lag |
 --------------------------------------------------------
 --------------------------------------------------------
 | default-templates-commands-results | 0         | 1   |
 --------------------------------------------------------
 | default-templates-commands-results | 1         | 0   |
 --------------------------------------------------------
 | default-templates-commands-results | 2         | 1   |
 --------------------------------------------------------

 Count: 3
 ```
