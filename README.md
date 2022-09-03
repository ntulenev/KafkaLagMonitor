# KafkaLagMonitor
Kafka lag monitoring utility (initial version)

Based on discussion at https://github.com/confluentinc/confluent-kafka-dotnet/issues/748

Config example:
```yaml
  "BootstrapServersConfiguration": {
    "BootstrapServers": [
      "test"
    ],
    "Username": "user123",
    "Password": "pwd123",
    "SecurityProtocol": "SaslPlaintext",
    "SASLMechanism": "ScramSha512"
  },
  "LagApplicationConfiguration": {
    "Timeout": "00:00:02",
    "Groups": [
      "commands"
    ]
  }
```

| Parameter name | Description   |
| -------------- | ------------- |
| BootstrapServers | List of kafka cluster servers, like "kafka-test:9092"  |
| Username | SASL username (optional)  |
| Password | SASL password (optional)  |
| SecurityProtocol | Protocol used to communicate with brokers (Plaintext,Ssl,SaslPlaintext,SaslSsl) (optional)  |
| SASLMechanism | SASL mechanism to use for authentication (Gssapi,Plain,ScramSha256,ScramSha512,OAuthBearer) (optional)  |
| Timeout | Cluster metadata loading timeout  |
| Groups | List of Kafka groups  |

Output example:
```
Group - commands
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
