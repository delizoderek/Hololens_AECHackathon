//
// Copyright 2014-2015 Amazon.com, 
// Inc. or its affiliates. All Rights Reserved.
// 
// Licensed under the Amazon Software License (the "License"). 
// You may not use this file except in compliance with the 
// License. A copy of the License is located at
// 
//     http://aws.amazon.com/asl/
// 
// or in the "license" file accompanying this file. This file is 
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, express or implied. See the License 
// for the specific language governing permissions and 
// limitations under the License.
//
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using Amazon;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon.SQS;
using UnityEngine.UI;

namespace AWSSDK.Examples
{
	public class SQSExample : MonoBehaviour
	{



		//identity pool id for cognito credentials

		//changeThis
		private string IdentityPoolId = "us-east-1:fb7491b2-4b07-436e-8ba5-5b8ed0c8968f";

		public string CognitoIdentityRegion = RegionEndpoint.USEast1.SystemName;

		public Controller controller;
		public GameObject gaze;
        public Material outlineMat;
		private GameObject cube;
		private string direction;


		private RegionEndpoint _CognitoIdentityRegion
		{
			get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
		}

		public string SQSRegion = RegionEndpoint.USEast1.SystemName;

		public void setObject(GameObject target){
			cube = target;
		}

		private void moveCube(string direction){
			switch(direction){
			case "left":
				cube.transform.Translate (-Vector3.right * 10f * Time.deltaTime);
				break;
			case "right":
				cube.transform.Translate (Vector3.right * 10f * Time.deltaTime);
				break;
			case "up":
				cube.transform.Translate (Vector3.up * 10f * Time.deltaTime);
				break;
			case "down":
				cube.transform.Translate (-Vector3.up * 10f * Time.deltaTime);
				break;
			}
		}

		private void rotateObj(string direction){
			switch(direction){
			case "clockwise":
				cube.transform.Rotate (0,0,90f);
				break;
			case "counterclockwise":
				cube.transform.Rotate (0,0, -90f);
				break;
			}
		}

		private RegionEndpoint _SQSRegion
		{
			get { return RegionEndpoint.GetBySystemName(SQSRegion); }
		}


		//name of the queue you want to create

		//changeThis
		private string QueueName = "colorexpert";

		private AWSCredentials _credentials;

		private AWSCredentials Credentials
		{
			get
			{
				if (_credentials == null)
					_credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
				return _credentials;
			}
		}

		private IAmazonSQS _sqsClient;

		private IAmazonSQS SqsClient
		{
			get
			{
				if (_sqsClient == null)
					_sqsClient = new AmazonSQSClient(Credentials, _SQSRegion);
				return _sqsClient;
			}
		}

		/*
		public Button CreateQueue;
		public Button SendMessage;
		public Button RetrieveMessage;
		public Button DeleteQueue;
		public InputField Message;
		*/

		//changeThis
		private string queueUrl = "https://sqs.us-east-1.amazonaws.com/952004314182/colorexpert"+"/?Action=SetQueueAttributes&Attribute.Name=ReceiveMessageWaitTimeSeconds&Attribute.Value=20";

		// Use this for initialization
		void Start()
		{
			//controller = GameObject.Find ("controller").GetComponent<Controller>();
			//controller.register (this.gameObject);

			UnityInitializer.AttachToGameObject(this.gameObject);
			/*
			CreateQueue.onClick.AddListener(CreateQueueListener);
			SendMessage.onClick.AddListener(SendMessageListener);
			RetrieveMessage.onClick.AddListener(RetrieveMessageListener);
			DeleteQueue.onClick.AddListener(DeleteQueueListener);
			*/


			StartCoroutine(RepeatRetrieveMessage(0.1F));
		}

		private void CreateQueueListener()
		{
			//            SqsClient.CreateQueueAsync(QueueName, (result) =>
			//            {
			//                if (result.Exception == null)
			//                {
			//                    Debug.Log(@"Queue Created");
			//                    queueUrl = result.Response.QueueUrl;
			//                }
			//                else
			//                {
			//                    Debug.LogException(result.Exception);
			//                }
			//            });
		}

		private void DeleteQueueListener()
		{
			//            if (!string.IsNullOrEmpty(queueUrl))
			//            {
			//                SqsClient.DeleteQueueAsync(queueUrl, (result) =>
			//                {
			//                    if (result.Exception == null)
			//                    {
			//                       Debug.Log(@"Queue Deleted");
			//                    }
			//                    else
			//                    {
			//                        Debug.LogException(result.Exception);
			//                    }
			//                });
			//            }
			//            else
			//            {
			//                Debug.Log(@"Queue Url is empty, make sure that the queue is created first");
			//            }
		}
		/*
		private void SendMessageListener()
		{

			if (!string.IsNullOrEmpty(queueUrl))
			{
				var message = Message.text;
				if (string.IsNullOrEmpty(message))
				{
					Debug.Log("No Message to send");
					return;
				}

				SqsClient.SendMessageAsync(queueUrl, message, (result) =>
					{
						if (result.Exception == null)
						{
							Debug.Log("Message Sent");
						}
						else
						{
							Debug.LogException(result.Exception);
						}
					});
			}
			else
			{
				Debug.Log(@"Queue Url is empty, make sure that the queue is created first");
			}
		}
		*/



		IEnumerator RepeatRetrieveMessage(float waitTime) {
			bool checkSQS = true;
			while (checkSQS) 
			{
				yield return new WaitForSeconds(waitTime);

				if (!string.IsNullOrEmpty (queueUrl)) {
					SqsClient.ReceiveMessageAsync (queueUrl, (result) => {
						if (result.Exception == null) {
							//Read the message
							var messages = result.Response.Messages;
							messages.ForEach (m => {
								Debug.Log (@"Message Id  = " + m.MessageId);
								Debug.Log (@"Mesage = " + m.Body);

								//Process the message
								string[] response = m.Body.Split('/');
								response = response[1].Contains(",") ? response[1].Split(',') : reverse(response);
								Debug.Log(response[0]);
								switch(response[0]){
								case "select":
									cube = gaze.GetComponent<GazeGestureManager>().getFocus();
                                        cube.GetComponent<Renderer>().material = outlineMat;
									break;
								case "rotate":
									if(cube != null){
										rotateObj(response[1]);
									}
									break;
								 case "move":
									if(cube != null){
										moveCube(response[1]);
									}
									break;
								//case "right":
									//break;
								//case "up":
									//break;
								//case "down":
									//break;
								//case "flip":
									//break;
								//case "on": 
									//break;
								//case "off":
									//break; 
								}

								//Delete the message
								var delRequest = new Amazon.SQS.Model.DeleteMessageRequest {
									QueueUrl = queueUrl,
									ReceiptHandle = m.ReceiptHandle

								};

								SqsClient.DeleteMessageAsync (delRequest, (delResult) => {
									if (delResult.Exception == null) {
									} else {
									}
								});




							});

						} else {
							Debug.LogException (result.Exception);
						}


					});
				} else {
					Debug.Log (@"Queue Url is empty, make sure that the queue is created first");
				}

				//Debug.Log (".");
			}
		}


		private void RetrieveMessageListener()
		{
			StartCoroutine(RepeatRetrieveMessage(0.1F));


		}

		private string[] reverse(string[] fix){
			string[] temp = new string[fix.Length];
			temp [0] = fix [1];
			temp [1] = fix [0];
			fix = temp;
			return fix;
		}

	}

}
