import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns

# Load the dataset
df = pd.read_csv('restructured_fivepointstestWithDatetime.csv')

# Convert 'datetime' column to datetime data type
df['datetime'] = pd.to_datetime(df['datetime'])

# Define the number of students to process
number_of_students_to_process = 10

# Get unique student IDs
unique_student_ids = df['student_id'].unique()[:number_of_students_to_process]

for user_id in unique_student_ids:
    # Filter DataFrame for the current user
    user_df = df[df['student_id'] == user_id].copy()

    # Sort the DataFrame by datetime
    user_df = user_df.sort_values(by='datetime', ascending=True)

    # Reset index to create new sequential index
    user_df.reset_index(drop=True, inplace=True)

    # Calculate time differences in seconds
    user_df['time_seconds'] = (user_df['datetime'] - user_df['datetime'].min()).dt.total_seconds()

    # Calculate the time difference between consecutive rows
    user_df['time_seconds_difference'] = user_df['time_seconds'].diff().fillna(0)

    # Create a figure with two subplots
    fig, axs = plt.subplots(2, 1, figsize=(10, 12))

    # Plot the histogram on the first subplot
    sns.histplot(user_df['time_seconds'], kde=True, ax=axs[0])
    axs[0].set_xlabel('Time (seconds)')
    axs[0].set_ylabel('Number of Submissions')
    axs[0].set_title('Submissions Over Time')
    axs[0].grid(True)

    # Plot the line plot on the second subplot
    axs[1].plot(user_df.index, user_df['time_seconds_difference'], marker='o', linestyle='-')
    axs[1].set_xlabel('Submission Index')
    axs[1].set_ylabel('Time (seconds)')
    axs[1].set_title('Time difference of Submission by Figure')
    axs[1].grid(True)

    # Adjust layout
    plt.tight_layout()

    # Save the figure
    plt.savefig(f'results_graph/user_{user_id}_figures.png')
    plt.clf()

print("Every figure has been drawn.")
