﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace Ex03_FacebookApp
{
    public partial class FriendsAndAlbumsForm : FacebookForm
    {
        private ImageList m_ImageList = new ImageList();
        private List<string> m_ImageListUrls = new List<string>();

        public FriendsAndAlbumsForm()
        {
            InitializeComponent();
        }

        public override void FetchData(User i_LoggedInUser)
        {
            base.FetchData(i_LoggedInUser);
            new Thread(fetchAlbumsList).Start();
            fetchFriendsList();
        }

        protected override void ButtonMatchFinder_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            base.ButtonMatchFinder_Click(sender, e);
        }

        protected override void ButtonProfile_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            base.ButtonProfile_Click(sender, e);
        }

        private void fetchAlbumsList()
        {
            int albumIndex = 0;
            m_ImageList.ImageSize = new Size(40, 40);
            foreach (Album album in LoggedInUser.Albums)
            {
                m_ImageListUrls.Add(album.PictureSmallURL);

                WebClient fetchImageUsingUrl = new WebClient();
                byte[] imageByte = fetchImageUsingUrl.DownloadData(m_ImageListUrls[albumIndex]);
                MemoryStream stream = new MemoryStream(imageByte);
                waitForControlToBeCreated();
                Image newImage = Image.FromStream(stream);
                listView1.Invoke(new Action(() => m_ImageList.Images.Add(newImage)));
                waitForControlToBeCreated();
                listViewSelectedAlbumPhotos.Invoke(new Action(() =>
                listViewSelectedAlbumPhotos.Items.Add(album.Name, albumIndex)));
                albumIndex++;
            }

            waitForControlToBeCreated();
            listViewSelectedAlbumPhotos.Invoke(new Action(() =>
            listViewSelectedAlbumPhotos.LargeImageList = m_ImageList));
        }

        private void waitForControlToBeCreated()
        {
            while (!this.IsHandleCreated)
            {
                Thread.Sleep(200);
            }
        }

        private void fetchAlbumData(int i_AlbumIndex)
        {
            textBoxAlbumCommentsCount.Text =
                LoggedInUser.Albums[i_AlbumIndex].Comments.Count.ToString();
            textBoxAlbumCreationTime.Text =
                LoggedInUser.Albums[i_AlbumIndex].CreatedTime.ToString();
            textBoxAlbumLikesCount.Text =
                LoggedInUser.Albums[i_AlbumIndex].LikedBy.Count.ToString();
            textBoxAlbumOwner.Text =
                LoggedInUser.Albums[i_AlbumIndex].From.Name;
        }

        private void listViewSelectedAlbumPhotos_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int albumIndex = 0;
            m_ImageList.ImageSize = new Size(40, 40);
            try
            {
                if (listViewSelectedAlbumPhotos.SelectedIndices.Count > 0)
                {
                    listView1.Clear();
                    int selectedIndex = listViewSelectedAlbumPhotos.SelectedIndices[0];
                    for (int i = 0; i < LoggedInUser.Albums[selectedIndex].Count; i++)
                    {
                        fetchAlbumData(selectedIndex);
                        m_ImageListUrls.Add(LoggedInUser.Albums[selectedIndex].PictureSmallURL);

                        WebClient fetchImageUsingUrl = new WebClient();
                        byte[] imageByte = fetchImageUsingUrl.DownloadData(m_ImageListUrls[albumIndex]);
                        MemoryStream stream = new MemoryStream(imageByte);

                        Image newImage = Image.FromStream(stream);
                        m_ImageList.Images.Add(newImage);

                        listView1.Items.Add(string.Empty, albumIndex);
                        albumIndex++;
                    }

                    listView1.LargeImageList = m_ImageList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fetchFriendsList()
        {
            listBoxFriendsList.Items.Clear();
            listBoxFriendsList.DisplayMember = "Name";
            foreach (User friend in LoggedInUser.Friends)
            {
                listBoxFriendsList.Items.Add(friend);
                friend.ReFetch(DynamicWrapper.eLoadOptions.Full);
            }

            displayNumberOfFriends();
        }

        private void displaySelectedFriend()
        {
            if (listBoxFriendsList.SelectedItems.Count == 1)
            {
                User selectedFriend = listBoxFriendsList.SelectedItem as User;
                if (selectedFriend.PictureNormalURL != null)
                {
                    pictureBoxSelectedFriend.Invoke(new Action(() =>
                    pictureBoxSelectedFriend.LoadAsync(selectedFriend.PictureNormalURL)));
                }
                else
                {
                    pictureBoxSelectedFriend.Invoke(new Action(() =>
                    pictureBoxSelectedFriend.Image = pictureBoxSelectedFriend.ErrorImage));
                }
            }
        }

        private void displayNumberOfFriends()   
        {
            waitForControlToBeCreated();
            textBoxFriendsCounter.Invoke(new Action(() => 
            textBoxFriendsCounter.Text = listBoxFriendsList.Items.Count.ToString()));
        }

        private void listBoxFriendsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            displaySelectedFriend();
        }
    }
}
