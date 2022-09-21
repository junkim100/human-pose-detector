import cv2
import mediapipe as mp
import time
import matplotlib.pyplot as plt

# Initialize mediapipe solutions
mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_pose = mp.solutions.pose
pose = mp_pose.Pose(static_image_mode=True)
mp_holistic = mp.solutions.holistic
holistic = mp_holistic.Holistic(min_detection_confidence=0.5, min_tracking_confidence=0.5)

fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')
plt.ion()

def static(solution):
  # Set image path
  img = cv2.imread("./data/image/istockphoto-1184187907-170667a.jpg")
  imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

  # Select solution
  if solution == "pose":
    results = pose.process(imgRGB)
  elif solution == "holistic":
    results = holistic.process(imgRGB)
  else:
    print("Invalid parameter\nTerminating program\n")

  feature_name = ["nose", "left eye inner", "left eye", "left eye outer", "right eye outer", "right eye", "right eye outer", "left ear", "right ear", "mouth left", "mouth right", "left shoulder", "right shoulder", "left elbow", "right elbow", "left wrist", "right wrist", "let pinky", "right pinky", "left index", "right index", "left thumb", "right thumb", "left hip", "right hip", "left knee", "right knee", "left ankle", "right ankle", "left heel", "right heel", "left foot index", "right foot index"]
  # feature x, y, z, visibility, feature_name
  feature = [[],[],[],[], feature_name]

  mp_drawing.draw_landmarks(img, results.pose_landmarks, mp_holistic.POSE_CONNECTIONS, landmark_drawing_spec=mp_drawing_styles.get_default_pose_landmarks_style())

  # Only for holistic
  # mp_drawing.draw_landmarks(img, results.face_landmarks, mp_holistic.FACEMESH_CONTOURS, landmark_drawing_spec=None, connection_drawing_spec=mp_drawing_styles.get_default_face_mesh_contours_style())

  for id, lm in enumerate(results.pose_landmarks.landmark):
    h, w, c = img.shape
    cx, cy = int(lm.x * w), int(lm.y * h)
    # Draw circle
    cv2.circle(img, (cx, cy), 5, (255, 0, 0), cv2.FILLED)

    # id is feature id
    # lm is x,y,z,visibility
    feature[0].append(lm.x)
    feature[1].append(lm.y)
    feature[2].append(lm.z)
    feature[3].append(lm.visibility)
    # print(feature_name[id], "\n", feature[2][id])

  cv2.imshow("Image", img)
  graph(feature)

def graph(feature):
  left_face = [ [feature[0][0], feature[0][1], feature[0][2], feature[0][3], feature[0][7]], 
                [feature[1][0], feature[1][1], feature[1][2], feature[1][3], feature[1][7]], 
                [feature[2][0], feature[2][1], feature[2][2], feature[2][3], feature[2][7]] ]
  right_face = [ [feature[0][0], feature[0][4], feature[0][5], feature[0][6], feature[0][8]], 
                 [feature[1][0], feature[1][4], feature[1][5], feature[1][6], feature[1][8]], 
                 [feature[2][0], feature[2][4], feature[2][5], feature[2][6], feature[2][8]] ]
  body = [ [feature[0][11], feature[0][12], feature[0][24], feature[0][23], feature[0][11]], 
           [feature[1][11], feature[1][12], feature[1][24], feature[1][23], feature[1][11]], [
            feature[2][11], feature[2][12], feature[2][24], feature[2][23], feature[2][11]] ]
  right_arm = [ [feature[0][12], feature[0][14], feature[0][16], feature[0][18], feature[0][20], feature[0][16], feature[0][22]], 
           [feature[1][12], feature[1][14], feature[1][16], feature[1][18], feature[1][20], feature[1][16], feature[1][22]], 
           [feature[2][12], feature[2][14], feature[2][16], feature[2][18], feature[2][20], feature[2][16], feature[2][22]] ]
  left_arm = [ [feature[0][11], feature[0][13], feature[0][15], feature[0][17], feature[0][19], feature[0][15], feature[0][21]], 
               [feature[1][11], feature[1][13], feature[1][15], feature[1][17], feature[1][19], feature[1][15], feature[1][21]], 
               [feature[2][11], feature[2][13], feature[2][15], feature[2][17], feature[2][19], feature[2][15], feature[2][21]] ]
  right_leg = [ [feature[0][24], feature[0][26], feature[0][28], feature[0][30], feature[0][32], feature[0][28]], 
                [feature[1][24], feature[1][26], feature[1][28], feature[1][30], feature[1][32], feature[1][28]], 
                [feature[2][24], feature[2][26], feature[2][28], feature[2][30], feature[2][32], feature[2][28]] ]
  left_leg = [ [feature[0][23], feature[0][25], feature[0][27], feature[0][29], feature[0][31], feature[0][27]], 
               [feature[1][23], feature[1][25], feature[1][27], feature[1][29], feature[1][31], feature[1][27]], 
               [feature[2][23], feature[2][25], feature[2][27], feature[2][29], feature[2][31], feature[2][27]] ]

  # plotting
  plot_shape = 'o'
  plot_color = 'b'
  # for i in range(0, len(feature[0])):
  ax.plot3D(left_face[0], left_face[2], left_face[1], marker=plot_shape, color=plot_color)
  ax.plot3D(right_face[0], right_face[2], right_face[1], marker=plot_shape, color=plot_color)
  ax.plot3D(body[0], body[2], body[1], marker=plot_shape, color=plot_color)
  ax.plot3D(right_arm[0], right_arm[2], right_arm[1], marker=plot_shape, color=plot_color)
  ax.plot3D(left_arm[0], left_arm[2], left_arm[1], marker=plot_shape, color=plot_color)
  ax.plot3D(right_leg[0], right_leg[2], right_leg[1], marker=plot_shape, color=plot_color)
  ax.plot3D(left_leg[0], left_leg[2], left_leg[1], marker=plot_shape, color=plot_color)

  # ax.set_xlim3d([-1.0, 0])
  # ax.set_ylim3d([1.0, -1.0])
  # ax.set_zlim3d([-3.0, -0.5])

  plt.show()
  plt.pause(0.00001)
  plt.cla()
  ax.set_axis_off()

def video(path,solution):
  cap = cv2.VideoCapture(path)
  pTime = 0
  while True:
      success, img = cap.read()
      if success == False:
          print("Failed to get video data")
          break

      imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

      # Select solution
      if solution == "pose":
        results = pose.process(imgRGB)
      elif solution == "holistic":
        results = holistic.process(imgRGB)
      else:
        print("Invalid parameter\nTerminating program\n")

      feature_name = ["nose", "left eye inner", "left eye", "left eye outer", "right eye outer", "right eye", "right eye outer", "left ear", "right ear", "mouth left", "mouth right", "left shoulder", "right shoulder", "left elbow", "right elbow", "left wrist", "right wrist", "let pinky", "right pinky", "left index", "right index", "left thumb", "right thumb", "left hip", "right hip", "left knee", "right knee", "left ankle", "right ankle", "left heel", "right heel", "left foot index", "right foot index"]
      # feature x, y, z, visibility, feature_name
      feature = [[],[],[],[], feature_name]

      if results.pose_landmarks:
          mp_drawing.draw_landmarks(img, results.pose_landmarks, mp_holistic.POSE_CONNECTIONS, landmark_drawing_spec=mp_drawing_styles.get_default_pose_landmarks_style())

          # Only for holistic
          # mp_drawing.draw_landmarks(img, results.face_landmarks, mp_holistic.FACEMESH_CONTOURS, landmark_drawing_spec=None, connection_drawing_spec=mp_drawing_styles.get_default_face_mesh_contours_style())

          for id, lm in enumerate(results.pose_landmarks.landmark):
            h, w, c = img.shape
            cx, cy = int(lm.x * w), int(lm.y * h)
            # Draw circle
            cv2.circle(img, (cx, cy), 5, (255, 0, 0), cv2.FILLED)

            # id is feature id
            # lm is x,y,z,visibility
            feature[0].append(lm.x*-1)
            feature[1].append(lm.y*-1)
            feature[2].append(lm.z)
            feature[3].append(lm.visibility)
            # print(feature_name[id], "\n", feature[2][id])

          # Flip video to give it a selfie look
          img = cv2.flip(img, 1)

          # Display FPS
          cTime = time.time()
          fps = 1 / (cTime - pTime)
          pTime = cTime
          cv2.putText(img, str(int(fps)), (70, 50), cv2.FONT_HERSHEY_PLAIN, 3, (255, 0, 0), 3)

          cv2.imshow("Image", img)
          graph(feature)

          # Exits window if "q" is pressed
          if cv2.waitKey(1) & 0xFF == ord('q'):
            break

  cap.release()
  cv2.destroyAllWindows()

def main():
  path = 0
  video(path,"pose")

if __name__=="__main__":
  main()